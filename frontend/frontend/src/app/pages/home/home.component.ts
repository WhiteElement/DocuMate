import { Component, OnInit } from '@angular/core';
import { DocumentOverview } from '../../../model/document-overview.model';
import { DocumentService } from '../../../service/document.service';
import { UploadstateService } from '../../../service/uploadstate.service';
import { Router } from '@angular/router';
import { DatePipe } from '@angular/common';
import { forkJoin, merge } from 'rxjs';
import { TagService } from '../../../service/tag.service';
import { Tag } from '../../../model/tag.model';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {

  documents: DocumentOverview[];
  tags: Tag[];

  constructor(private router: Router, private documentService: DocumentService, private uploadstateService: UploadstateService, private tagService: TagService) {

  }

  ngOnInit(): void {
    this.documentService.getAll().subscribe(docs => {
      this.documents = docs;
    });

    this.tagService.getAll().subscribe(tagResponse => {
      this.tags = tagResponse.body;
    });


    //forkJoin([allDocs, allTags]).subscribe(([docs, tagResponse]) => {
    //  this.documents = docs;
    //  this.tags = tagResponse.body;
    //})
  }


  toMetadataPage(fileInput: HTMLInputElement): void {
    const files = fileInput.files;

    this.uploadstateService.setCurrentFiles(Array.from(files));
    console.log("set", this.uploadstateService.getCurrentFiles());

    this.router.navigate(['metadata']);
  }

}
