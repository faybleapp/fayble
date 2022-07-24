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
						
							<CoverItem
								key={item.id}
								item={item}
								title={item.name || "Untitled"}
								firstSubtitle={`Volume ${
									item.volume ?? item.year
								}`}
								secondSubtitle={`${item.bookCount} Issues`}
								link={`/library/${item.library?.id}/series/${item.id}`}
							/>							
						
					);
				})}
		</div>
	);
};
