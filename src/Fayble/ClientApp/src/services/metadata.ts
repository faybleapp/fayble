import { BookResult, SeriesResult } from "models/api-models";
import { useApiQuery } from "./useApiQuery";


export const useSeriesMetadata = (id: string) =>
  useApiQuery<SeriesResult>(["seriesMetadata", id], `/metadata/series/${id}`);

  
export const useBookMetadata = (id: string) =>
useApiQuery<BookResult>(["bookMetadata", id], `/metadata/book/${id}`);
