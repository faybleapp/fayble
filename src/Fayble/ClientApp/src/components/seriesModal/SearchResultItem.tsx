import { Image } from "components/image";
import { SeriesSearchResult } from "models/api-models";
import styles from "./SearchResultItem.module.scss";

interface SearchResultItemProps {
  searchResult: SeriesSearchResult;
  onSelect: (searchResultId: string) => void;
}

export const SearchResultItem = ({
  searchResult,
  onSelect,
}: SearchResultItemProps) => {
  return (
    <div
      className={styles.searchResult}
      onClick={() => onSelect(searchResult.id)}>
      <div className={styles.coverContainer}>
        <Image className={styles.cover} src={searchResult.image || ""} />
      </div>
      <div className={styles.detailsContainer}>
        <h6>{searchResult.name}</h6>
        <div className={styles.description}>{searchResult.description}</div>
      </div>
    </div>
  );
};
