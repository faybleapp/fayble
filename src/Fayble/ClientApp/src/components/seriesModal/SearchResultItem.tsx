import { Image } from "components/image";
import { SeriesSearchResult } from "models/api-models";
import styles from "./SearchResultItem.module.scss";

interface SearchResultItemProps {
  searchResult: SeriesSearchResult;
}

export const SearchResultItem = ({ searchResult }: SearchResultItemProps) => {
  return (
    <div className={styles.searchResult}>
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
