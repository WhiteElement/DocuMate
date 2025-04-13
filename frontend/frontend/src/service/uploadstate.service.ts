import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UploadstateService {
  private currentFiles: File[] = [];

  constructor() { }


  setCurrentFiles(files: File[]): void {
    this.currentFiles = files;
  }

  getCurrentFiles(): File[] {
    return this.currentFiles;
  }

  reset(): void {
    this.currentFiles = [];
  }
}
