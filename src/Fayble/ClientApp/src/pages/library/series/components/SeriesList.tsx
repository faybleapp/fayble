// @ts-nocheck
import {
  ColumnDef,
  createColumnHelper,
  flexRender,
  getCoreRowModel,
  useReactTable
} from "@tanstack/react-table";
import { Series } from "models/api-models";
import React from "react";
import { useNavigate, useParams } from "react-router-dom";

interface SeriesListProps {
  items: Series[];
}

export const SeriesList = ({ items }: SeriesListProps) => {
  const { libraryId } = useParams<{ libraryId: string }>();
  const navigate = useNavigate();

  const columnHelper = createColumnHelper<Series>();

  //   const columns: Column<Series>[] = React.useMemo(
  //     () => [
  //       {
  //         Header: "Name",
  //         accessor: "name",
  //         isSortable: true,
  //       },
  //       {
  //         Header: "Year",
  //         accessor: "year",
  //         isSortable: true,
  //       },
  //       {
  //         Header: "Volume",
  //         accessor: "volume",
  //         isSortable: true,
  //       },
  //       {
  //         Header: "Book Count",
  //         accessor: "bookCount",
  //         isSortable: true,
  //       },
  //     ],
  //     []
  //   );

  
  const columns: ColumnDef<Series, any>[] = [    
    columnHelper.accessor("year", {
      header: "Name",
      cell: (info) => info.renderValue(),
      footer: (info) => info.column.id,
    }),
  ];
  const rerender = React.useReducer(() => ({}), {})[1];
  const data = React.useMemo(() => items, [items]);

  const table = useReactTable({
    data,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  return (
    <div className="p-2">
      <table>
        <thead>
          {table.getHeaderGroups().map((headerGroup) => (
            <tr key={headerGroup.id}>
              {headerGroup.headers.map((header) => (
                <th key={header.id}>
                  {header.isPlaceholder
                    ? null
                    : flexRender(
                        header.column.columnDef.header,
                        header.getContext()
                      )}
                </th>
              ))}
            </tr>
          ))}
        </thead>
        <tbody>
          {table.getRowModel().rows.map((row) => (
            <tr key={row.id}>
              {row.getVisibleCells().map((cell) => (
                <td key={cell.id}>
                  {flexRender(cell.column.columnDef.cell, cell.getContext())}
                </td>
              ))}
            </tr>
          ))}
        </tbody>
        <tfoot>
          {table.getFooterGroups().map((footerGroup) => (
            <tr key={footerGroup.id}>
              {footerGroup.headers.map((header) => (
                <th key={header.id}>
                  {header.isPlaceholder
                    ? null
                    : flexRender(
                        header.column.columnDef.footer,
                        header.getContext()
                      )}
                </th>
              ))}
            </tr>
          ))}
        </tfoot>
      </table>
      <div className="h-4" />
      <button onClick={() => rerender()} className="border p-2">
        Rerender
      </button>
    </div>
  );
};
