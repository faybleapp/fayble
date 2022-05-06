import { BookItem } from "components/bookItem";
import { Book } from "models/api-models";
import { MediaType } from "models/ui-models";
import React from "react";
import styles from "./BookCoverGrid.module.scss";

interface BookCoverGridProps {
	books: Book[];
	title?: string;
}

export const BookCoverGrid = ({ books, title }: BookCoverGridProps) => {
	// const { libraryId, seriesId } = useParams<{
	// 	libraryId: string;
	// 	seriesId: string;
	// }>();

	// const markRead = (id: string) => {};

	// const edit = (id: string) => {};

	return (
		<>
			<div className={styles.gridTitle}>
				<h6>{title}</h6>
			</div>
			{books &&
				books.map((book) => {
					return (
						<BookItem
							book={book}
							title={
								book.mediaType === MediaType.Book
									? book.title
									: `Issue #${book.number.padStart(3, "0")}`
							}
							subtitle={`${book.pageCount} pages`}
						/>
					);
				})}
		</>
	);
};
