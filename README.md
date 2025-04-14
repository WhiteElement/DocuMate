# DocuMate – Dokumentenmanagement mit Docuemnt Scan, OCR & Tagging

**DocuMate** ist eine Webanwendung zur Organisation, Verschlagwortung und Volltextsuche von PDF-Dokumenten.
Abfotografierte Dokumente sind die Basis hierfür. Diese können hochgeladen werden, woraufhin eine Perspektivkorrektur vorgenommen wird, ein Pdf erzeugt und OCR durchgefürt wird.

---

## Features

- **Dokumentensuche** mit Tag-Facetten
- **Tag-Verwaltung** mit Zuweisung, Entfernung und Erstellung
- **Datei-Upload via Drag & Drop**
- **Optical Character Recognition** des Textes aus gescannten PDFs
- **Metadatenerfassung** (Name, Beschreibung, Tags) direkt vor Upload

---
## 📸 Screenshots

| Hauptansicht | Detailansicht | Upload |
|--------------|----------------|--------|
| ![](https://github.com/user-attachments/assets/1ec9c323-5d7d-4a0a-b6f1-a0d59c564a09) | ![](https://github.com/user-attachments/assets/1066fe9c-2c68-4fbe-8ed4-1626e5d75f87) | ![](https://github.com/user-attachments/assets/b545b5a5-36ef-43a1-b030-02cff48ce719) |


## Tech Stack

| Layer         | Technologien                                         |
|---------------|------------------------------------------------------|
| Frontend      | Angular, HTML, CSS          |
| Backend       | C# .NET Web API (C#), MongoDB, OpenCv, Tesseract OCR |

---

## Meine Idee

Privatversicherte müssen ihre Arztrechnungen selbst einreichen und überweisen. Je nach beteiligten Parteien, verliert man sehr schnell die übersicht darüber welches Dokument wo bereits eingereicht und von welcher Partei bereits gezahlt wurde.
Das ist der Grund, warum ich DocuMate geschrieben habe. Die Basis der Funktionalität ist das dynamische Tag-System. Später habe ich zusätzliche Ideen wie die Volltextsuche durch OCR gehabt.
Mittlerweile benutze ich die Software zur Digitalisierung aller Dokumente. MongoDB und das Tag-System sind so dynamisch, das es sich nicht nur auf den ursprünglichen use-case beschränkt.

---

## Lokales Setup
Das Repo enthält sowohl das FE als auch das BE.
Nach dem clonen, muss das FE gebuildet werden (die statischen Files werden automatisch richtig abgelegt), dann das BE gepublished werden und danach mit einem .env-File gestartet werden.

```bash
git clone (https://github.com/WhiteElement/DocuMate.git
cd frontend\frontend
ng build
cd ..\..\backend\DokuMate
dotnet publish
# nun die Exe mit einem .env File als ersten Parameter starten

# .env
CONNECTION_STRING="yourmongodbconnectionstring"
DATABASE_NAME="yourdatabasename"
