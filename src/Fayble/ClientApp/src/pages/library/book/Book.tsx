import { Container } from "components/container";
import { BreadcrumbItem, LibraryView } from "models/ui-models";
import React, { useState } from "react";
import { useParams } from "react-router-dom";
import { useBook } from "services/book";
import { LibraryHeader } from "../LibraryHeader";
import { BookDetail } from "./BookDetail";

export const Book = () => {
	const { libraryId, seriesId, bookId } = useParams<{
		libraryId: string;
		seriesId: string;
		bookId: string;
	}>();

	const { data: book, isLoading: isLoadingBook } = useBook(bookId!);

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
				</>
			)}
		</Container>
	);
};
