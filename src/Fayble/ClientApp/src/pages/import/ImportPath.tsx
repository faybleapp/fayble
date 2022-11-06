import { useState } from "react";
import { Button, Spinner } from "react-bootstrap";
import styles from "./ImportPath.module.scss";

interface ImportPathProps {
  onSearch: (path: string) => void;
  isSearching: boolean;
}

export const ImportPath = ({ onSearch, isSearching }: ImportPathProps) => {
  const [path, setPath] = useState<string>("");
  return (
    <div className={styles.container}>
      <input
        placeholder="Import Path"
        type="text"
        disabled={isSearching}
        className={styles.pathInput}
        onChange={(e: React.FormEvent<HTMLInputElement>) => {
          setPath(e.currentTarget.value);
        }}
      />
      <Button
        disabled={!path.trim() || isSearching}
        onClick={() => onSearch(path)}
        className={styles.searchButton}>
        {isSearching ? (
          <Spinner
            as="span"
            animation="border"
            size="sm"
            role="status"
            aria-hidden="true"
          />
        ) : (
          "Search"
        )}
      </Button>
    </div>
  );
};
