import { ImportScanFile, ImportScanRequest } from "models/api-models";
import { useQueryClient } from "react-query";
import { useApiMutation } from "./useApiMutation";

export const useScanImportFiles = () => {
  const queryClient = useQueryClient();
  return useApiMutation<ImportScanFile[], { path: string }>(
    "POST",
    () => `/import/scan`,
    {
      onSuccess: () => {
        queryClient.invalidateQueries("import");
      },
    }
  );
};

export const useImportFiles = () => {
  return useApiMutation<null, ImportScanRequest[]>("POST", () => `/import`);
};
