import { SanitisePath } from "helpers/pathHelpers";
import React, { useState } from "react";
import { Button, Form, InputGroup } from "react-bootstrap";
import { useFormContext } from "react-hook-form";
import { toast } from "react-toastify";
import { useHttpClient } from "services/httpClient";
import styles from "./LibraryPathsTab.module.scss";

export const LibraryPathTab = () => {
  const [isValidatingPath, setValidatingPath] = useState(false);
  const [newPath, setNewPath] = useState("");
  const client = useHttpClient();

  const {
    register,
    setValue,
    watch,
    formState: { errors },
  } = useFormContext();

  const field = register("folderPath");
  const folderPath = watch("folderPath");

  const removePath = () => {
    setValue("folderPath", "", { shouldDirty: true });
  };

  const pathExists = async (path: string) => {
    return (await client.get<boolean>(`/filesystem/pathexists?path=${path}`))
      .data;
  };

  const addPath = async () => {
    setValidatingPath(true);

    const sanitisedPath = SanitisePath(newPath);

    let valid = false;
    try {
      valid = await pathExists(sanitisedPath);
    } catch (error) {
      toast.error("An error occured while validating path");
      setValidatingPath(false);
      return;
    }

    if (!valid) {
      toast.error("Path does not exist or is not accessible");
      setValidatingPath(false);
      return;
    }
    setValue("folderPath", newPath, { shouldDirty: true });

    setNewPath("");
    setValidatingPath(false);
  };

  return (
    <div className={styles.path}>
      {!folderPath ? (
        <>
          <Form.Group className={"mb-3"}>
            <InputGroup>
              <Form.Control
                disabled={isValidatingPath}
                onChange={(e: React.ChangeEvent<HTMLInputElement>): void =>
                  setNewPath(e.target.value)
                }
                value={newPath}
                placeholder="Path"
              />
              <Button
                disabled={isValidatingPath || newPath.trim() === ""}
                onClick={() => addPath()}
                variant="outline-secondary">
                {isValidatingPath ? "Validating..." : "Add Path"}
              </Button>
            </InputGroup>
          </Form.Group>
        </>
      ) : (
        <InputGroup className={styles.pathItem}>
          <Form.Control disabled placeholder="Path" value={folderPath} />
          <Button onClick={() => removePath()} variant="danger">
            X
          </Button>
        </InputGroup>
      )}
    </div>
  );
};
