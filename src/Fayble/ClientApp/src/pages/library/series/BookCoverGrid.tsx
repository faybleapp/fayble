import { CoverItem } from "components/coverItem";
import { Book } from "models/api-models";
import { MediaType } from "models/ui-models";
import styles from "./BookCoverGrid.module.scss";

interface BookCoverGridProps {
	books: Book[];
	title?: string;
}

export const BookCoverGrid = ({ books, title }: BookCoverGridProps) => {
	return (
		<>
			<div className={styles.gridTitle}>
				<h6>{title}</h6>
			</div>
			{books &&
				books.map((book) => {
					return (
						<CoverItem
							item={book}
							title={
								book.mediaType === MediaType.Book
									? book.title
									: `Issue #${book.number.padStart(3, "0")}`
							}
							firstSubtitle={`${book.pageCount} pages`}
							link={`/library/${book.library?.id}/series/${book.series?.id}/book/${book.id}`}
							isDeleted={book.isDeleted}
						/>
					);
				})}
		</>
	);
};
