import { Component, OnInit } from '@angular/core';
import { DocumentService } from '../../service/document.service';
import { DocumentOverview } from '../../model/document-overview.model';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-documentdetail',
  standalone: true,
  imports: [],
  templateUrl: './documentdetail.component.html',
  styleUrl: './documentdetail.component.css'
})
export class DocumentdetailComponent implements OnInit {

  document: DocumentOverview;

  constructor(private documentService: DocumentService, private route: ActivatedRoute) {

  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');

      this.documentService.getOne(id).subscribe(doc => {
        this.document = doc;
        console.log("document", this.document);
      })
    });
  }


}
