import {
  ColumnDef,
  createColumnHelper,
  flexRender,
  getCoreRowModel,
  useReactTable
} from "@tanstack/react-table";
import { ComicFile, ImportFile } from "models/api-models";
import { useState } from "react";
import { useAllSeries } from "services";
import { SelectSeriesModal } from "./components/SelectSeriesModal";

interface ImportTableProps {
  files: ComicFile[];
}

export const ImportTable = ({ files }: ImportTableProps) => {
  const [showSeriesModal, setShowSeriesModal] = useState<boolean>(false);
  const [selectedFiles, setSelectedFiles] = useState<Array<string>>([]);
  const [importFiles, setImportFiles] = useState<Array<ImportFile>>(
    files.map((file) => {
      return {
        seriesId: "",
        destinationFileName: "",
        filePath: file.filePath,
      };
    })
  );
  const [singleSelectedSeries, setSingleSelectedSeries] = useState<
    string | undefined
  >();
  const { data: series } = useAllSeries();

  const columnHelper = createColumnHelper<ComicFile>();

  const renderSeriesSelect = (id: string) => {
    const importFile = importFiles.find((f) => f.filePath === id);
    return !!importFile?.seriesId ? (
      importFiles.find((f) => f.filePath === id)?.seriesId
    ) : (
      <div
        onClick={() => {
          setSingleSelectedSeries(id);
          setShowSeriesModal(true);
        }}>
        Select Series
      </div>
    );
  };

  const renderCheckbox = (id: string) => {
    return (
      <input
        type="checkbox"
        checked={selectedFiles.includes(id)}
        onChange={(e) => {
          e.target.checked
            ? setSelectedFiles([...selectedFiles, id])
            : setSelectedFiles(selectedFiles.filter((item) => item !== id));
        }}
      />
    );
  };

  const columns: ColumnDef<ComicFile, any>[] = [
    columnHelper.accessor((row) => row, {
      id: "check",
      header: "",
      cell: (info) => renderCheckbox(info.row.original.filePath),
    }),
    columnHelper.accessor((row) => row.fileName, {
      id: "filename",
      header: "Filename",
      cell: (info) => info.getValue(),
    }),
    columnHelper.accessor((row) => row, {
      header: "Series",
      cell: (info) => renderSeriesSelect(info.row.original.filePath),
    }),
  ];

  const table = useReactTable({
    data: files,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });
  return (
    <>
      <div className="p-2">
        <table>
          <thead>
            {table.getHeaderGroups()?.map((headerGroup) => (
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
      </div>
      <SelectSeriesModal
        show={showSeriesModal}
        onClose={() => {
          setShowSeriesModal(false);
          setSingleSelectedSeries(undefined);
        }}
        onSelectSeries={(seriesId) => {
          setImportFiles(
            importFiles.map((file) => {
              if (file.filePath === singleSelectedSeries) {
                return { ...file, seriesId: seriesId };
              }
              return file;
            })
          );
          setShowSeriesModal(false);
        }}
      />
    </>
  );
};
