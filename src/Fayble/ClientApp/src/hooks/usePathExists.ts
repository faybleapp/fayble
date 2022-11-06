import { useHttpClient } from "services/httpClient";

export const usePathExists = async (path: string) => {
  const client = useHttpClient();
  return (await client.get<boolean>(`/filesystem/pathexists?path=${path}`))
    .data;
};
