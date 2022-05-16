import { BookHorizontalScrollList } from "components/bookHorizontalScrollList";
import { BookModal } from "components/bookModal";
import { LibraryHeader } from "components/libraryHeader";
import { PageContainer } from "components/pageContainer";
import { getBookType } from "helpers/bookHelpers";
import { BreadcrumbItem, ViewType } from "models/ui-models";
import { useState } from "react";
import { useParams } from "react-router-dom";
import { useBook, useRelatedBooks } from "services/book";
import { BookDetail } from "./BookDetail";

export const Book = () => {
	const { libraryId, bookId } = useParams<{
		libraryId: string;
		seriesId: string;
		bookId: string;
	}>();

	const { data: book, isLoading: isLoadingBook } = useBook(bookId!);
	const { data: relatedBooks } =
		useRelatedBooks(bookId!);

	const [showBookModal, setShowBookModal] = useState<boolean>(false);
	//const [ view, setView] = useState<ViewType>(ViewType.CoverGrid);

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
		<PageContainer loading={isLoadingBook}>
			{book && (
				<>
					<LibraryHeader
						libraryId={libraryId!}
						libraryView={ViewType.CoverGrid}
						navItems={breadCrumbItems}
						changeView={() => {}}
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
		</PageContainer>
	);
};
