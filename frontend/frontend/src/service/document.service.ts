import { Injectable } from '@angular/core';
import { DocumentOverview } from '../model/document-overview.model';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Searchrequest } from '../model/searchrequest.model';

@Injectable({
  providedIn: 'root'
})
export class DocumentService {
  private baseUrl: string = 'api/document';

  constructor(private http: HttpClient) { }

  getAll(): Observable<DocumentOverview[]> {
    return this.http.get<DocumentOverview[]>(this.baseUrl)
  }

  getAllFiltered(doc: Searchrequest): Observable<DocumentOverview[]> {
    return this.http.post<DocumentOverview[]>(`${this.baseUrl}/search`, JSON.stringify(doc), {
      headers: {
        "Content-Type": "application/json"
      }
    });
  }

  getOne(id: string): Observable<DocumentOverview> {
    if (id === '')
      return null;

    const url = `${this.baseUrl}/${id}`;
    return this.http.get<DocumentOverview>(url)
  }

  createOne(formData: FormData): Observable<DocumentOverview> {
    return this.http.post<DocumentOverview>(this.baseUrl, formData);
  }

  download(id: string): Observable<Blob> {
    return this.http.get(`${this.baseUrl}/${id}/download`, {
      responseType: 'blob'
    })
  }


  updateOne(document: DocumentOverview): Observable<HttpResponse<Object>> {
    const url = `${this.baseUrl}/${document.id}`;
    return this.http.put(url, JSON.stringify(document), {
      headers: {
        "Content-Type": "application/json"
      },
      observe: 'response'
    });
  }
}
