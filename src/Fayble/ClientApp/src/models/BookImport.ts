export interface BookImport {
  id: string
  seriesId: string | undefined;  
  seriesMatchId: string | undefined;
  destinationFileName: string;
  matchId: string | undefined;
  filePath: string;
  number: string | undefined;
  checked: boolean;  
  loading: boolean;  
  fileName: string;
  exists: boolean;  
}
