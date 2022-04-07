import { Tag } from "models/api-models";
import { useApiQuery } from "services/useApiQuery";

export const useBookTags = () => useApiQuery<Tag[]>(["bookTags"], `/tags/booktags`)