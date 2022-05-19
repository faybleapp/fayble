import { Person } from "models/api-models";
import { useApiQuery } from "services/useApiQuery";

export const usePeople = () => useApiQuery<Person[]>(["people"], `/people`)