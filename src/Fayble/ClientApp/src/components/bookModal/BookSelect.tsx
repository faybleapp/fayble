import { LoadingIndicator } from "components/loadingIndicator";
import { SelectField } from "components/selectField";
import { Container } from "react-bootstrap";
import { useSeriesMetadata } from "services";
import styles from "./BookSelect.module.scss";

interface BookSelectProps {
  seriesMatchId: string;
  onMatch: (matchId: string) => void;
}

export const BookSelect = ({ seriesMatchId, onMatch }: BookSelectProps) => {  
  const { data: seriesMetadata, isLoading: isLoadingMetadata } =
    useSeriesMetadata(seriesMatchId);

  return (
    <Container>
      {isLoadingMetadata ? (
        <div className={styles.loadingIndicator}>
          <LoadingIndicator />
        </div>
      ) : (
        <>
          <SelectField
            onChange={(matchId) => onMatch(matchId)}
            clearable
            name="series"
            options={
              seriesMetadata?.books?.map((book) => ({
                value: book.id,
                label: `${book.number.padStart(3, "0")}${
                  book.title && " - " + book.title
                }`,
              })) || []
            }
          />
        </>
      )}
    </Container>
  );
};
