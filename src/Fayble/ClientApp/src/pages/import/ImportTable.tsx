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
import {
  IndeterminateCheckbox,
  IndeterminateCheckboxValue
} from "components/indeterminateCheckbox";
import { ComicFile, ImportFile } from "models/api-models";
import { useEffect, useState } from "react";
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
  const [checkedFiles, setCheckedFiles] = useState<Array<string>>([]);
  const [selectedFile, setSelectedFile] = useState<string | undefined>();
  const [selectAllStatus, setSelectAllStatus] =
    useState<IndeterminateCheckboxValue>(IndeterminateCheckboxValue.Unchecked);
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

  const { data: series } = useAllSeries();

  useEffect(() => {
    if (checkedFiles.length === 0) {
      setSelectAllStatus(IndeterminateCheckboxValue.Unchecked);
    } else if (checkedFiles.length === importFiles.length) {
      setSelectAllStatus(IndeterminateCheckboxValue.Checked);
    } else {
      setSelectAllStatus(IndeterminateCheckboxValue.Indeterminate);
    }
  }, [checkedFiles, importFiles]);

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
          setSelectedFile(id);
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
        checked={checkedFiles.includes(id)}
        onChange={(e) => {
          e.target.checked
            ? setCheckedFiles([...checkedFiles, id])
            : setCheckedFiles(checkedFiles.filter((item) => item !== id));
        }}
      />
    );
  };

  const renderHeaderCheckbox = () => {
    return (
      <IndeterminateCheckbox
        onChange={() => {
          setCheckedFiles(
            selectAllStatus === IndeterminateCheckboxValue.Checked ||
              selectAllStatus === IndeterminateCheckboxValue.Indeterminate
              ? []
              : importFiles.map((i) => i.filePath)
          );
        }}
        value={selectAllStatus}
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
          setSelectedFile(id);
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
          setSelectedFile(id);
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
      header: () => renderHeaderCheckbox(),
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
          setSelectedFile(undefined);
        }}
        onSelectSeries={(seriesId) => {
          setImportFiles(
            importFiles.map((file) => {
              // Process single file
              if (selectedFile) {
                if (file.filePath === selectedFile) {
                  return { ...file, seriesId: seriesId };
                }
                return file;
              }
              // Process checked files
              if (checkedFiles.some((f) => f === file.filePath)) {
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
          importFiles.find((f) => f.filePath === selectedFile)?.number || ""
        }
        show={showNumberModal}
        onClose={() => {
          setShowNumberModal(false);
          setSelectedFile(undefined);
        }}
        onChange={(number) => {
          setImportFiles(
            importFiles.map((file) => {
              if (file.filePath === selectedFile) {
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
          setSelectedFile(undefined);
        }}
        show={showImportFilenameModal}
        filename={
          importFiles.find((f) => f.filePath === selectedFile)
            ?.destinationFileName || ""
        }
        onChange={(fileName) => {
          setImportFiles(
            importFiles.map((file) => {
              if (file.filePath === selectedFile) {
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
