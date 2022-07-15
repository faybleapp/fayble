import { SeriesSearchResult } from "models/api-models";
import { MetadataSearchQuery } from "models/ui-models";
import { useApiQuery } from "./useApiQuery";

export const useSearchSeries = ({ year, name }: MetadataSearchQuery) =>
	useApiQuery<SeriesSearchResult[]>(
		["metadata", name, year],
		`/metadata/searchseries?name=${name}&year=${year}`
	);
