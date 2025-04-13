import { Tag } from "./tag.model";

export class DocumentOverview {
  id: string;
  name: string;
  info: string
  tags: Tag[];
  created: Date;
  ocrContent: string;
}
