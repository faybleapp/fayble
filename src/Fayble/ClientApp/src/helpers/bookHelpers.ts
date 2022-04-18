import { MediaType } from "models/ui-models";

export const getBookType = (mediaType: string) =>
	mediaType === MediaType.ComicBook ? "Issue" : "Book";
