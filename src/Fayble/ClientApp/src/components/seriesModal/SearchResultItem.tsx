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
        <div className={styles.details}>
          {searchResult?.publisher && (
            <div className={styles.detailProperty}>
              <div className={styles.detailsHeading}>Publisher</div>
              <div>{searchResult?.publisher}</div>
            </div>
          )}
          {searchResult?.startYear && (
            <div className={styles.detailProperty}>
              <div className={styles.detailsHeading}>Year</div>
              <div>{searchResult?.startYear}</div>
            </div>
          )}
          {!!searchResult.issueCount ? (
            <div className={styles.detailProperty}>
              <div className={styles.detailsHeading}>Issues</div>
              <div>{searchResult?.issueCount}</div>
            </div>
          ) : null}
        </div>
      </div>
    </div>
  );
};
