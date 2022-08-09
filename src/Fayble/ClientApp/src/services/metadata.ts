import { SeriesResult } from "models/api-models";
import { useApiQuery } from "./useApiQuery";


export const useSeriesMetadata = (id: string) =>
  useApiQuery<SeriesResult>(["seriesMetadata", id], `/metadata/series/${id}`);
