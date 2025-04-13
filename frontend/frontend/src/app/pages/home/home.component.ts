import { Component } from '@angular/core';
import { DocumentOverview } from '../../../model/document-overview.model';
import { DocumentService } from '../../../service/document.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

  documents: DocumentOverview[];

  constructor(private documentService: DocumentService) {
    documentService.getAll().subscribe(docs => this.documents = docs);

  }

}
