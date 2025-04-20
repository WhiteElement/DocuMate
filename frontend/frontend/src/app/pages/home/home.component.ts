import { Component, NgModule, OnInit } from '@angular/core';
import { DocumentOverview } from '../../../model/document-overview.model';
import { DocumentService } from '../../../service/document.service';
import { UploadstateService } from '../../../service/uploadstate.service';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { TagService } from '../../../service/tag.service';
import { Tag } from '../../../model/tag.model';
import { FormsModule } from '@angular/forms';
import { Searchrequest } from '../../../model/searchrequest.model';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [DatePipe, FormsModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {

  documents: DocumentOverview[];
  tags: Tag[];
  selectedTags: Tag[] = [];
  searchBar: string;

  constructor(private router: Router, private documentService: DocumentService, private uploadstateService: UploadstateService, private tagService: TagService) {

  }

  // TODO: add errorhandling for when no response is comming => no db conn)
  ngOnInit(): void {
    this.search();

    this.tagService.getAll().subscribe(tagResponse => {
      this.tags = tagResponse.body;
    });
  }

  search(): void {
    if (this.fullSearch()) {
      this.documentService.getAll().subscribe(docs => {
        this.documents = docs;
      });
    } else {
      const filter: Searchrequest = { Name: this.searchBar || "", Tags: this.selectedTags };
      this.documentService.getAllFiltered(filter).subscribe(docs => {
        this.documents = docs;
      });
    }
  }

  fullSearch(): boolean {
    return (this.searchBar === undefined || this.searchBar === '')
      && (this.selectedTags === undefined || this.selectedTags.length === 0);
  }


  toMetadataPage(fileInput: HTMLInputElement): void {
    const files = fileInput.files;

    this.uploadstateService.setCurrentFiles(Array.from(files));
    this.router.navigate(['metadata']);
  }

  toDetailsPage(id: string) {
    this.router.navigate(['/document', id]);
  }

  toggleSelection(tag: Tag) {
    const idx = this.selectedTags.indexOf(tag);
    if (idx !== -1) {
      this.selectedTags.splice(idx);
    } else {
      this.selectedTags.push(tag)
    }
  }

}
