import { CoverItem } from "components/coverItem";
import { Series } from "models/api-models";

interface SeriesCoverGridProps {
	items: Series[];
}

export const SeriesCoverGrid = ({ items }: SeriesCoverGridProps) => {
	return (
		<div>
			{items &&
				items.map((item) => {
					return (
						<>
							<CoverItem
								item={item}
								title={item.name || "Untitled"}
								firstSubtitle={`Volume ${
									item.volume ?? item.year
								}`}
								secondSubtitle={`${item.bookCount} Issues`}
								link={`/series/${item.id}`}
							/>							
						</>
					);
				})}
		</div>
	);
};
