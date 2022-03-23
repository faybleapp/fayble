import Notfound from "assets/notfound2.svg";
import { Container } from "components/container";
import { LibraryModal } from "components/libraryModal";
import { BreadcrumbItem, LibraryView } from "models/ui-models";
import React, { useState } from "react";
import { Image } from "react-bootstrap";
import { useParams } from "react-router-dom";
import { useLibrary, useLibrarySeries } from "services/library";
import styles from "./Library.module.scss";
import { LibraryHeader } from "./LibraryHeader";
import { SeriesCoverGrid } from "./SeriesCoverGrid";

export const Library = () => {
	const { libraryId } = useParams<{ libraryId: string }>();
	const { data: library } = useLibrary(libraryId!);
	const { data: series } = useLibrarySeries(libraryId!);
	const [showLibraryModal, setShowLibraryModal] = useState<boolean>(false);

	const breadCrumbItems: BreadcrumbItem[] = [
		{
			name: (library && library?.name) || "",
			link: `/library/${libraryId}`,
			active: true,
		},
	];

	return (
		<Container>
			{library && (
				<>
					<LibraryHeader
						libraryId={libraryId!}
						libraryView={LibraryView.CoverGrid}
						navItems={breadCrumbItems}
						openEditModal={() => setShowLibraryModal(true)}
					/>
					{series && series.length === 0 ? (
						<>
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
							<div className={styles.emptyLibraryText}></div>
						</>
					) : (
						<SeriesCoverGrid items={series || []} />
					)}
					<LibraryModal
						show={showLibraryModal}
						close={() => setShowLibraryModal(false)}
						library={library}
					/>
				</>
			)}
		</Container>
	);
};
