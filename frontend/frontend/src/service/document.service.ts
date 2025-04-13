import { Injectable } from '@angular/core';
import { DocumentOverview } from '../model/document-overview.model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DocumentService {
  private baseUrl: string = 'document';

  constructor(private http: HttpClient) { }

  getAll(): Observable<DocumentOverview[]> {
    return this.http.get<DocumentOverview[]>(this.baseUrl)
  }


}
