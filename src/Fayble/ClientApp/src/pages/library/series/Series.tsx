import { LibraryHeader } from "components/libraryHeader";
import { PageContainer } from "components/pageContainer";
import { SeriesModal } from "components/seriesModal";
import { BreadcrumbItem, ViewType } from "models/ui-models";
import { useState } from "react";
import { useParams } from "react-router-dom";
import { useSeries, useSeriesBooks } from "services/series";
import { BookCoverGrid } from "./components/BookCoverGrid";
import { SeriesDetail } from "./components/SeriesDetail";
import styles from "./Series.module.scss";

export const Series = () => {
	const { seriesId } = useParams<{
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
			link: `/library/${series?.library?.id}`,
		},
		{
			name: (series && series?.name) || "",
			link: `/library/${series?.library?.id}/series/${seriesId}`,
			active: true,
		},
	];

	return (
		<PageContainer loading={isLoadingSeries || isLoadingBooks}>
			{series && (
				<>
					<LibraryHeader
						libraryId={series?.library?.id!}
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
		</PageContainer>
	);
};
