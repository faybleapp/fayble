import { Container } from "components/container";
import { LibraryHeader } from "components/libraryHeader";
import { SeriesModal } from "components/seriesModal";
import { BreadcrumbItem, ViewType } from "models/ui-models";
import React, { useState } from "react";
import { useParams } from "react-router-dom";
import { useSeries, useSeriesBooks } from "services/series";
import { BookCoverGrid } from "./BookCoverGrid";
import styles from "./Series.module.scss";
import { SeriesDetail } from "./SeriesDetail";

export const Series = () => {
	const { libraryId, seriesId } = useParams<{
		libraryId: string;
		seriesId: string;
	}>();

	const { data: series, isLoading: isLoadingSeries } = useSeries(seriesId!);
	const { data: books, isLoading: isLoadingBooks } = useSeriesBooks(
		seriesId!
	);

	const [showSeriesModal, setShowSeriesModal] = useState<boolean>(false);
	//const [view, setView] = useState<ViewType>(ViewType.CoverGrid);

	const breadCrumbItems: BreadcrumbItem[] = [
		{
			name: (series && series?.library?.name) || "",
			link: `/library/${libraryId}`,
		},
		{
			name: (series && series?.name) || "",
			link: `/library/${libraryId}/series/${seriesId}`,
			active: true,
		},
	];

	return (
		<Container loading={isLoadingSeries || isLoadingBooks}>
			{series && (
				<>
					<LibraryHeader
						libraryId={libraryId!}
						libraryView={ViewType.CoverGrid}
						navItems={breadCrumbItems}
						changeView={() => {}}
						openEditModal={() => setShowSeriesModal(true)}
					/>
					<div className={styles.seriesBody}>
						<SeriesDetail series={series} />
						<BookCoverGrid books={books || []} title="Issues" />
						<SeriesModal
							show={showSeriesModal}
							series={series}
							close={() => setShowSeriesModal(false)}
						/>
					</div>
				</>
			)}
		</Container>
	);
};
