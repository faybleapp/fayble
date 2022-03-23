import { Publisher } from "models/api-models";
import { useApiQuery } from "services/useApiQuery";

export const usePublisher = (id: string) => useApiQuery<Publisher>(["publisher", id], `/publishers/${id}`);

export const usePublishers = () => useApiQuery<Publisher[]>(["publishers"], `/publishers`)