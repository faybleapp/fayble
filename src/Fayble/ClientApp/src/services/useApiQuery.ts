import { QueryKey, useQuery, UseQueryOptions } from "react-query";
import { useHttpClient } from "./httpClient";

export const useApiQuery = <T>(key: QueryKey, path: string, config?: UseQueryOptions<T | null>) => {

    const client = useHttpClient();

    return useQuery(key, async () => (await client.get<T>(path)).data, config);
}
