import { PathValidation } from "models/api-models";
import { useApiMutation } from "./useApiMutation";

export const usePathExists = () => {
  return useApiMutation<Boolean, null, PathValidation>(
    "POST",
    () => `/filesystem/pathexists`
  );
};