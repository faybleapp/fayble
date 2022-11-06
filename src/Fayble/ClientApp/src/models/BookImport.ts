import { BookImportStatus } from "./BookImportStatus";

export interface BookImport {
  id: string;
  seriesId: string;
  seriesMatchId: string | undefined;
  destinationFileName: string;
  matchId: string | undefined;
  filePath: string;
  number: string | undefined;
  checked: boolean;  
  fileName: string;
  exists: boolean;
  status: BookImportStatus;
}
