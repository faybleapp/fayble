import { Book } from "models/api-models";
import { useEffect } from "react";
import { Container } from "react-bootstrap";
import { useFormContext, useWatch } from "react-hook-form";
import { useRefreshBookMetadata } from "services";
import { BookMatch } from "./BookMatch";
import { BookSelect } from "./BookSelect";

interface BookMatchTabProps {
  book: Book;
}

export const BookMatchTab = ({ book }: BookMatchTabProps) => {
  const { control, setValue, formState } = useFormContext();
  const matchedBookId = useWatch({ control, name: "matchId" });
  const { mutate: refreshMetadata, isLoading: isRefreshingMetadata } =
    useRefreshBookMetadata(book.id);
  const handleRefreshMetadata = () => {};
  const setMatchId = (id?: string) => {
    console.log(id);
    setValue("matchId", id, { shouldDirty: true });
  };

  useEffect(() => {
    console.log(book.matchId);
  }, [book.matchId]);
  return (
    <Container>
      {!book?.series?.matchId ? (
        <div>Series must be matched before book can be matched.</div>
      ) : matchedBookId ? (
        <BookMatch
          isRefreshingMetadata={isRefreshingMetadata}
          bookMatchId={matchedBookId}
          showRefreshMetadata={
            !formState.dirtyFields?.matchId && book.matchId != undefined
          }
          onRefreshMetadata={handleRefreshMetadata}
          onUnmatchBook={() => setMatchId(undefined)}
        />
      ) : (
        <BookSelect
          seriesMatchId={book.series.matchId}
          onMatch={(matchId) => {
            setMatchId(matchId);
          }}
        />
      )}
    </Container>
  );
};
