import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { DocumentmetadataComponent } from './pages/documentmetadata/documentmetadata.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'metadata', component: DocumentmetadataComponent }
];
