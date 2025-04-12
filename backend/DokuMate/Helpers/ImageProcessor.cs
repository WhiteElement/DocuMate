using OpenCvSharp;
using Point = OpenCvSharp.Point;
using Size = OpenCvSharp.Size;

namespace DokuMate.Helpers;

public class ImageProcessor
{
    public bool GenerateIntermediateFiles { get; set; }
    public FileInfo[] InputFiles { get; set; }
    public DirectoryInfo OutputFilePath { get; set; }


    /// <summary>
    /// Performs a document scan operation on the input images.
    /// </summary>
    /// <returns>An array of output file paths.</returns>
    public string[] DocumentScan()
    {
        return InputFiles
            .Select(file =>
            {
                if (string.IsNullOrEmpty(file.Name) || HasFalseFileExtension(file.Name))
                    throw new ArgumentException($"inputFilePath {file.Name} does not have the right file extension (.jpg, .jpeg, .png)");

                using Mat inputImage = Cv2.ImRead(file.FullName);
                using Mat preprocessedImage = PreProcess(inputImage);
                List<Point[]> contours = FindTopContours(preprocessedImage);

                if (GenerateIntermediateFiles)
                    GenerateContourImage(inputImage, contours, file);

                Point[] contourPoints = FindBiggestContour(contours);
                Point[] sortedPoints = SortPoints(contourPoints);

                using Mat warpedImage = WarpImage(inputImage, sortedPoints);
                string fileName = FileName("output", file);

                if (Cv2.ImWrite(fileName, warpedImage))
                    return fileName;

                throw new InvalidOperationException("Could not write Document Scan File");
            })
            .ToArray();
    }

    private string FileName(string prefix, System.IO.FileInfo file)
    {
        return Path.Combine(OutputFilePath.FullName, $"{prefix}_{file.Name}");
    }

    private Mat WarpImage(Mat inputImage, Point[] contourPoints)
    {
        var warpedSize = CalculateDocumentSize(contourPoints);
        var pts1 = new Point2f[] { contourPoints[0], contourPoints[1], contourPoints[2], contourPoints[3] };
        var pts2 = new Point2f[]
        {
            new (0, 0),
            new (warpedSize.Width, 0),
            new (0, warpedSize.Height),
            new (warpedSize.Width, warpedSize.Height)
        };

        using Mat matrix = Cv2.GetPerspectiveTransform(pts1, pts2);
        using Mat warpedImage = new Mat();
        Cv2.WarpPerspective(inputImage, warpedImage, matrix, warpedSize);

        return warpedImage.Clone();
    }

    private Size CalculateDocumentSize(Point[] contourPoints)
    {
        double width = Math.Sqrt(
            Math.Pow(contourPoints[0].X - contourPoints[1].X, 2) +
            Math.Pow(contourPoints[0].Y - contourPoints[1].Y, 2)
        );
        double height = Math.Sqrt(
            Math.Pow(contourPoints[0].X - contourPoints[2].X, 2) +
            Math.Pow(contourPoints[0].Y - contourPoints[2].Y, 2)
        );

        return new Size(width, height);
    }

    private void GenerateContourImage(Mat inputImage, List<Point[]> contours, System.IO.FileInfo file)
    {
        using Mat contourImage = inputImage.Clone();
        Cv2.DrawContours(contourImage, contours, -1, new Scalar(255, 0, 255), 3);
        Cv2.ImWrite(FileName("contour", file), contourImage);
    }

    private Point[] FindBiggestContour(List<Point[]> contours)
    {
        double maxArea = 0;
        Point[] documentContour = null;

        foreach (var contour in contours)
        {
            double area = Cv2.ContourArea(contour);
            if (area > 1000)
            {
                double peri = Cv2.ArcLength(contour, true);
                var approx = Cv2.ApproxPolyDP(contour, 0.015 * peri, true);

                if (area > maxArea && approx.Length == 4)
                {
                    documentContour = approx;
                    maxArea = area;
                }
            }
        }

        return documentContour ?? throw new InvalidOperationException("No valid contour found");
    }

    private List<Point[]> FindTopContours(Mat inputImage)
    {
        using var threshold = new Mat();
        Cv2.Threshold(inputImage, threshold, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);

        var contours = Cv2.FindContoursAsArray(threshold, RetrievalModes.List, ContourApproximationModes.ApproxSimple);
        return contours
            .OrderByDescending(c => Cv2.ContourArea(c))
            .ToList();
    }

    private Mat PreProcess(Mat inputImage)
    {
        using var gray = new Mat();
        using var blurred = new Mat();
        Cv2.CvtColor(inputImage, gray, ColorConversionCodes.BGR2GRAY);
        Cv2.GaussianBlur(gray, blurred, new OpenCvSharp.Size(5, 5), 0);
        return blurred.Clone();
    }

    private Point[] SortPoints(Point[] points)
    {
        var sortedByY = points.OrderBy(p => p.Y).ToArray();
        var sortedPoints = new Point[4];

        if (sortedByY[0].X < sortedByY[1].X)
        {
            sortedPoints[0] = sortedByY[0];
            sortedPoints[1] = sortedByY[1];
        }
        else
        {
            sortedPoints[0] = sortedByY[1];
            sortedPoints[1] = sortedByY[0];
        }

        if (sortedByY[2].X < sortedByY[3].X)
        {
            sortedPoints[2] = sortedByY[2];
            sortedPoints[3] = sortedByY[3];
        }
        else
        {
            sortedPoints[2] = sortedByY[3];
            sortedPoints[3] = sortedByY[2];
        }

        return sortedPoints;
    }

    private bool HasFalseFileExtension(string file)
    {
        return !file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) &&
               !file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) &&
               !file.EndsWith(".png", StringComparison.OrdinalIgnoreCase);
    }
}