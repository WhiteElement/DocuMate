import { Component } from '@angular/core';
import { UploadstateService } from '../../../service/uploadstate.service';
import { DocumentService } from '../../../service/document.service';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-documentmetadata',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './documentmetadata.component.html',
  styleUrl: './documentmetadata.component.css'
})
export class DocumentmetadataComponent {
  metadata = {
    Name: '',
    Info: '',
  }

  files: File[];

  constructor(private uploadstateService: UploadstateService, private documentService: DocumentService, private router: Router) {
    this.files = this.uploadstateService.getCurrentFiles();
    console.log("inmetadata", this.uploadstateService.getCurrentFiles());
  }

  uploadDocument(): void {
    const formData = new FormData();
    formData.append('Name', this.metadata.Name);
    formData.append('Info', this.metadata.Info);
    this.files.forEach(f => formData.append('Images', f));

    this.documentService.createOne(formData).subscribe(doc => {
      this.router.navigate(['/document', doc.id]);
    });
  }

}
