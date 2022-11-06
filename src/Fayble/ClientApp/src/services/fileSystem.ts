import { FileExistsRequest, GenerateFilenameRequest, PathValidation } from "models/api-models";
import { useApiMutation } from "./useApiMutation";

export const usePathExists = () => {
  return useApiMutation<Boolean, PathValidation>(
    "POST",
    () => `/filesystem/pathexists`
  );
};

export const useGenerateFilename = () => {  
  return useApiMutation<string, GenerateFilenameRequest>(
    "POST",
    () => `/filesystem/generate-filename`
  );
};

export const useFileExists = () => {  
  return useApiMutation<boolean, FileExistsRequest>(
    "POST",
    () => `/filesystem/file-exists`
  );
};
