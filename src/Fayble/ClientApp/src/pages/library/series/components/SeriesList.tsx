import { faChevronDown, faChevronUp } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Series } from "models/api-models";
import React from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Column, useSortBy, useTable } from "react-table";
import styles from "./SeriesList.module.scss";

interface SeriesListProps {
	items: Series[];
}

export const SeriesList = ({ items }: SeriesListProps) => {
	const { libraryId } = useParams<{ libraryId: string }>();
	const navigate = useNavigate();

	const columns: Column<Series>[] = React.useMemo(
		() => [
			{
				Header: "Name",
				accessor: "name",
				isSortable: true,
			},
			{
				Header: "Year",
				accessor: "year",
				isSortable: true,
			},
			{
				Header: "Volume",
				accessor: "volume",
				isSortable: true,
			},
			{
				Header: "Book Count",
				accessor: "bookCount",
				isSortable: true,
			},
		],
		[]
	);

	const data = React.useMemo(() => items, [items]);

	const tableInstance = useTable({ columns, data }, useSortBy);

	const { getTableProps, getTableBodyProps, headerGroups, rows, prepareRow } =
		tableInstance;

	return (
		<table {...getTableProps()} className={styles.table}>
			<thead>
				{headerGroups.map((headerGroup) => (
					<tr {...headerGroup.getHeaderGroupProps()}>
						{headerGroup.headers.map((column) => (
							<th
								className={styles.header}
								{...column.getHeaderProps(
									column.getSortByToggleProps()
								)}>
								{column.render("Header")}
								<span>
									{column.isSorted ? (
										column.isSortedDesc ? (
											<FontAwesomeIcon
												icon={faChevronDown}
												className={styles.sortIcon}
												size="sm"
											/>
										) : (
											<FontAwesomeIcon
												icon={faChevronUp}
												className={styles.sortIcon}
												size="sm"
											/>
										)
									) : (
										""
									)}
								</span>
							</th>
						))}
					</tr>
				))}
			</thead>
			<tbody {...getTableBodyProps()}>
				{rows.map((row) => {
					prepareRow(row);
					return (
						<tr {...row.getRowProps()} className={styles.row} onClick={() => navigate(`/library/${libraryId}/series/${row.original.id}`)}>
							{row.cells.map((cell) => {
								return (
									<td
										className={styles.cell}
										{...cell.getCellProps()}
										style={{
											padding: "10px",
											border: "solid 1px gray",
											background: "papayawhip",
										}}>
										{cell.render("Cell")}
									</td>
								);
							})}
						</tr>
					);
				})}
			</tbody>
		</table>
	);
};
