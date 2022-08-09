import Notfound from "assets/NotFound.svg";
import { LibraryHeader } from "components/libraryHeader";
import { LibraryModal } from "components/libraryModal";
import { PageContainer } from "components/pageContainer";
import { BreadcrumbItem, ViewType } from "models/ui-models";
import { useState } from "react";
import { Image } from "react-bootstrap";
import { useParams } from "react-router-dom";
import { useLibrary, useLibrarySeries } from "services/library";
import { SeriesCoverGrid } from "./components/SeriesCoverGrid";
import styles from "./Library.module.scss";
import { SeriesList } from "./series/components/SeriesList";

export const Library = () => {
	const { libraryId } = useParams<{ libraryId: string }>();
	const { data: library, isLoading: isLoadingLibrary } = useLibrary(
		libraryId!
	);
	const { data: series, isLoading: isLoadingSeries } = useLibrarySeries(
		libraryId!
	);
	const [showLibraryModal, setShowLibraryModal] = useState<boolean>(false);
	const [view, setView] = useState<ViewType>(ViewType.CoverGrid);

	const breadCrumbItems: BreadcrumbItem[] = [
		{
			name: (library && library?.name) || "",
			link: `/library/${libraryId}`,
			active: true,
		},
	];

	return (
		<PageContainer loading={isLoadingLibrary || isLoadingSeries}>
			{library && (
				<>
					<LibraryHeader
						libraryId={libraryId!}
						libraryView={view}
						navItems={breadCrumbItems}
						changeView={setView}
						openEditModal={() => setShowLibraryModal(true)}
					/>
					{series && series.length === 0 ? (
						<div className={styles.emptyLibraryImageContainer}>
							<div>
								<Image
									className={styles.emptyLibraryImage}
									src={Notfound}
									alt="React Logo"
								/>
								<p className={styles.emptyLibraryText}>
									There are no items in this library.
								</p>
							</div>
						</div>
					) : view === ViewType.CoverGrid ? (
						<SeriesCoverGrid items={series || []} />
					) : (
						<SeriesList items={series || []} />
					)}
					<LibraryModal
						show={showLibraryModal}
						close={() => setShowLibraryModal(false)}
						library={library}
					/>
				</>
			)}
		</PageContainer>
	);
};
