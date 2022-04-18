import { BookHorizontalScrollList } from "components/bookHorizontalScrollList";
import { BookModal } from "components/bookModal";
import { Container } from "components/container";
import { getBookType } from "helpers/bookHelpers";
import { BreadcrumbItem, LibraryView } from "models/ui-models";
import React, { useState } from "react";
import { useParams } from "react-router-dom";
import { useBook, useRelatedBooks } from "services/book";
import { LibraryHeader } from "../LibraryHeader";
import { BookDetail } from "./BookDetail";

export const Book = () => {
	const { libraryId, seriesId, bookId } = useParams<{
		libraryId: string;
		seriesId: string;
		bookId: string;
	}>();

	const { data: book, isLoading: isLoadingBook } = useBook(bookId!);
	const { data: relatedBooks, isLoading: isLoadingRelatedBooks } =
		useRelatedBooks(bookId!);

	const [showBookModal, setShowBookModal] = useState<boolean>(false);

	const breadCrumbItems: BreadcrumbItem[] = [
		{
			name: (book && book?.library?.name) || "",
			link: `/library/${book?.library?.id}`,
		},
		{
			name: (book && book?.series?.name) || "",
			link: `/library/${book?.library?.id}/series/${book?.series?.id}`,
		},
		{
			name: (book && `Issue #${book?.number.padStart(3, "0")}`) || "",
			link: `/library/${book?.library?.id}/series/${book?.series?.id}/book/${book?.id}`,
			active: true,
		},
	];

	return (
		<Container loading={isLoadingBook}>
			{book && (
				<>
					<LibraryHeader
						libraryId={libraryId!}
						libraryView={LibraryView.CoverGrid}
						navItems={breadCrumbItems}
						openEditModal={() => setShowBookModal(true)}
					/>
					<BookDetail book={book} />
					{relatedBooks?.booksInSeries && (
						<BookHorizontalScrollList
							title={`Other ${getBookType(
								book.mediaType
							)}s in Series`}
							books={relatedBooks.booksInSeries}
						/>
					)}
					{relatedBooks?.booksByPublisher && (
						<BookHorizontalScrollList
							title={`Other ${getBookType(
								book.mediaType
							)}s by Publisher`}
							books={relatedBooks.booksByPublisher}
						/>
					)}
					{relatedBooks?.booksByAuthor && (
						<BookHorizontalScrollList
							title={`Other ${getBookType(
								book.mediaType
							)}s by author`}
							books={relatedBooks.booksByAuthor}
						/>
					)}
					{relatedBooks?.booksReleasedSameMonth && (
						<BookHorizontalScrollList
							title="Released Same Month"
							books={relatedBooks.booksReleasedSameMonth}
						/>
					)}
					{relatedBooks?.booksReleasedSameYear && (
						<BookHorizontalScrollList
							title="Released Same Year"
							books={relatedBooks.booksReleasedSameYear}
						/>
					)}
					

					<BookModal
						show={showBookModal}
						book={book}
						close={() => setShowBookModal(false)}
					/>
				</>
			)}
		</Container>
	);
};
