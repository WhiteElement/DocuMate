import { Component, OnInit } from '@angular/core';
import { DocumentService } from '../../service/document.service';
import { DocumentOverview } from '../../model/document-overview.model';
import { ActivatedRoute } from '@angular/router';
import { Tag } from '../../model/tag.model';
import { TagService } from '../../service/tag.service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-documentdetail',
  standalone: true,
  imports: [],
  templateUrl: './documentdetail.component.html',
  styleUrl: './documentdetail.component.css'
})
export class DocumentdetailComponent implements OnInit {

  document: DocumentOverview;
  tags: Tag[];

  constructor(private documentService: DocumentService, private tagService: TagService, private route: ActivatedRoute) {

  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');

      const getDoc = this.documentService.getOne(id);
      const getTags = this.tagService.getAll();

      forkJoin([getDoc, getTags]).subscribe(([doc, tagsResponse]) => {
        this.document = doc;

        const allTags = tagsResponse.body;
        const filteredTags = allTags.filter(t => !doc.tags.includes(t));
        this.tags = filteredTags;
      });
    });
  }

  addToDocument(tag: Tag) {

  }

  removeTag(tag: Tag) {

  }


}
