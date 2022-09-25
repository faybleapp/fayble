import { faCircleExclamation } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  ColumnDef,
  createColumnHelper,
  flexRender,
  getCoreRowModel,
  useReactTable
} from "@tanstack/react-table";
import cn from "classnames";
import { ComicFile, ImportFile } from "models/api-models";
import { useState } from "react";
import { OverlayTrigger, Table, Tooltip } from "react-bootstrap";
import { useAllSeries } from "services";
import { SelectSeriesModal } from "./components/SelectSeriesModal";
import { SetImportFilenameModal } from "./components/SetImportFilenameModal";
import { SetNumberModal } from "./components/SetNumberModal";
import styles from "./ImportTable.module.scss";

interface ImportTableProps {
  files: ComicFile[];
}

export const ImportTable = ({ files }: ImportTableProps) => {
  const [showSeriesModal, setShowSeriesModal] = useState<boolean>(false);
  const [showNumberModal, setShowNumberModal] = useState<boolean>(false);
  const [showImportFilenameModal, setShowImportFilenameModal] =
    useState<boolean>(false);
  const [selectedFiles, setSelectedFiles] = useState<Array<string>>([]);
  const [importFiles, setImportFiles] = useState<Array<ImportFile>>(
    files.map((file) => {
      return {
        seriesId: "",
        destinationFileName: file.fileName,
        filePath: file.filePath,
        number: file.number || "",
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
    const seriesName = series?.find((s) => s.id === importFile?.seriesId)?.name;
    return (
      <div
        className={cn(styles.selectableCell, {
          [styles.placeholder]: !seriesName,
        })}
        onClick={() => {
          setSingleSelectedSeries(id);
          setShowSeriesModal(true);
        }}>
        {seriesName ?? "select"}
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

  const renderImportFilename = (id: string) => {
    const importFile = importFiles.find((f) => f.filePath === id);
    return (
      <div
        className={cn(styles.selectableCell, {
          [styles.placeholder]: !importFile?.destinationFileName,
        })}
        onClick={() => {
          setSingleSelectedSeries(id);
          setShowImportFilenameModal(true);
        }}>
        {!!importFile?.destinationFileName
          ? importFile?.destinationFileName
          : "set"}
      </div>
    );
  };

  const renderNumber = (id: string) => {
    const importFile = importFiles.find((f) => f.filePath === id);
    return (
      <div
        className={cn(styles.selectableCell, {
          [styles.placeholder]: !importFile?.number,
        })}
        onClick={() => {
          setSingleSelectedSeries(id);
          setShowNumberModal(true);
        }}>
        {!!importFile?.number ? importFile?.number : "set"}
      </div>
    );
  };

  const renderValid = (id: string) => {
    const importFile = importFiles.find((f) => f.filePath === id);
    let valid = false;
    let error = "";
    if (!importFile?.seriesId) {
      error = "You must select a series";
    } else if (!importFile?.destinationFileName) {
      error = "You must set a destination filename";
    } else {
      valid = true;
    }
    return (
      !valid && (
        <OverlayTrigger placement="left" overlay={<Tooltip>{error}</Tooltip>}>
          <div>
            <FontAwesomeIcon
              className={styles.errorIcon}
              icon={faCircleExclamation}
            />
          </div>
        </OverlayTrigger>
      )
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
    columnHelper.accessor((row) => row, {
      header: "Number",
      cell: (info) => renderNumber(info.row.original.filePath),
    }),
    columnHelper.accessor((row) => row, {
      header: "Import Filename",
      cell: (info) => renderImportFilename(info.row.original.filePath),
    }),
    columnHelper.accessor((row) => row, {
      id: "valid",
      header: "",
      cell: (info) => renderValid(info.row.original.filePath),
    }),
  ];

  const table = useReactTable({
    data: files,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  return (
    <>
      <div>
        <Table striped hover>
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
          <tbody className={styles.tableBody}>
            {table.getRowModel().rows.map((row) => (
              <tr key={row.id}>
                {row.getVisibleCells().map((cell) => (
                  <td className={styles.tableCell} key={cell.id}>
                    {flexRender(cell.column.columnDef.cell, cell.getContext())}
                  </td>
                ))}
              </tr>
            ))}
          </tbody>
        </Table>
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
      <SetNumberModal
        number={
          importFiles.find((f) => f.filePath === singleSelectedSeries)
            ?.number || ""
        }
        show={showNumberModal}
        onClose={() => {
          setShowNumberModal(false);
          setSingleSelectedSeries(undefined);
        }}
        onChange={(number) => {
          setImportFiles(
            importFiles.map((file) => {
              if (file.filePath === singleSelectedSeries) {
                return { ...file, number: number };
              }
              return file;
            })
          );
          setShowSeriesModal(false);
        }}
      />

      <SetImportFilenameModal
        onClose={() => {
          setShowImportFilenameModal(false);
          setSingleSelectedSeries(undefined);
        }}
        show={showImportFilenameModal}
        filename={
          importFiles.find((f) => f.filePath === singleSelectedSeries)
            ?.destinationFileName || ""
        }
        onChange={(fileName) => {
          setImportFiles(
            importFiles.map((file) => {
              if (file.filePath === singleSelectedSeries) {
                return { ...file, destinationFileName: fileName };
              }
              return file;
            })
          );
        }}
      />
    </>
  );
};
