import { Tag } from "models/api-models";
import { useApiQuery } from "services/useApiQuery";

export const useTags = () => useApiQuery<Tag[]>(["tags"], `/tags`)