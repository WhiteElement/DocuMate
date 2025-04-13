import { Component } from '@angular/core';
import { DocumentOverview } from '../../../model/document-overview.model';
import { DocumentService } from '../../../service/document.service';
import { UploadstateService } from '../../../service/uploadstate.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

  documents: DocumentOverview[];

  constructor(private router: Router, private documentService: DocumentService, private uploadstateService: UploadstateService) {
    documentService.getAll().subscribe(docs => this.documents = docs);

  }

  toMetadataPage(fileInput: HTMLInputElement): void {
    const files = fileInput.files;

    this.uploadstateService.setCurrentFiles(Array.from(files));
    console.log("set", this.uploadstateService.getCurrentFiles());

    this.router.navigate(['metadata']);
  }

}
