import { Injectable } from '@angular/core';
import { Tag } from '../model/tag.model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HttpResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class TagService {
  private baseUrl: string = 'api/tag';

  constructor(private http: HttpClient) { }

  getAll(): Observable<HttpResponse<Tag[]>> {
    return this.http.get<Tag[]>(this.baseUrl, { observe: 'response' })
  }

  createOne(name: string): Observable<HttpResponse<Tag>> {
    const newTag: Tag = { name: name, id: '' };
    return this.http.post<Tag>(this.baseUrl, newTag, { observe: 'response' });
  }

  deleteOne(id: string): Observable<HttpResponse<Object>> {
    const url = `${this.baseUrl}/${id}`;
    return this.http.delete(url, { observe: 'response' });
  }
}
