import { Component, OnInit } from '@angular/core';
import { DocumentService } from '../../service/document.service';
import { DocumentOverview } from '../../model/document-overview.model';
import { ActivatedRoute } from '@angular/router';
import { Tag } from '../../model/tag.model';
import { TagService } from '../../service/tag.service';
import { forkJoin } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-documentdetail',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './documentdetail.component.html',
  styleUrl: './documentdetail.component.css'
})
export class DocumentdetailComponent implements OnInit {

  // TODO: pipe null-values in document.info

  document: DocumentOverview;

  tags: Tag[];

  showAddTagInput = false;
  newTagName = '';
  pdfUrl: SafeResourceUrl;
  showOcr = false;

  constructor(private documentService: DocumentService, private tagService: TagService, private route: ActivatedRoute, private sanitizer: DomSanitizer) {

  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      this.pdfUrl = `api/document/${id}/download`;

      const getDoc = this.documentService.getOne(id);
      const getTags = this.tagService.getAll();

      forkJoin([getDoc, getTags]).subscribe(([doc, tagsResponse]) => {
        this.document = doc;
        this.tags = this.remainingTags(doc, tagsResponse.body);
      });

      this.documentService.download(id).subscribe(blob => {
        const url = URL.createObjectURL(blob);
        this.pdfUrl = this.sanitizer.bypassSecurityTrustResourceUrl(url);

      })
    });
  }

  addToDocument(tag: Tag) {
    const docCopy = JSON.parse(JSON.stringify(this.document));
    docCopy.tags.push(tag);
    this.documentService.updateOne(docCopy).subscribe(res => {
      if (res.status === 200) {
        this.document = docCopy;
        this.tags = this.tags.filter(t => t != tag);
      }
    });
  }

  removeTag(tag: Tag) {
    const docCopy: DocumentOverview = JSON.parse(JSON.stringify(this.document));
    docCopy.tags = docCopy.tags.filter(t => t.name != tag.name);
    this.documentService.updateOne(docCopy).subscribe(res => {
      if (res.status === 200) {
        this.document = docCopy;
        this.tags = this.remainingTags(docCopy, this.tags);
      }
    });
  }

  remainingTags(document: DocumentOverview, tags: Tag[]) {
    const bodyTags = document.tags.map(x => x.name);
    const allTags = tags
    return allTags.filter(t => !bodyTags.includes(t.name));
  }

  toggleAddTagInput() {
    this.showAddTagInput = !this.showAddTagInput;
    this.newTagName = '';
  }

  createTag() {
    const trimmedInput = this.newTagName.trim();
    if (trimmedInput) {
      this.tagService.createOne(trimmedInput).subscribe(res => {
        if (res.status.toString().startsWith("2")) {
          this.tags.push(res.body);
          this.newTagName = '';
          this.showAddTagInput = false;
        }
      });
    }
  }

  updateDocument() {
    this.documentService.updateOne(this.document);
  }
}
