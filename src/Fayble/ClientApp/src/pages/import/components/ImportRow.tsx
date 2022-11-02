import { flexRender, Row } from "@tanstack/react-table";
import { BookImport } from "models";
import { useMemo } from "react";
import { toast } from "react-toastify";
import { useFileExists, useGenerateFilename } from "services/fileSystem";

interface ImportRowProps {
  row: Row<BookImport>;
  updateBookImport: (updatedFile: BookImport) => void;
}

export const ImportRow = ({ row, updateBookImport }: ImportRowProps) => {
  const generateFilename = useGenerateFilename();
  const fileExists = useFileExists();
  const file = row.original;

  useMemo(() => {
    if (!file?.seriesId || !file?.number) return;

    updateBookImport({
      ...file,
      loading: true,
    });
    generateFilename.mutate(
      {
        seriesId: file.seriesId,
        bookMatchId: file?.matchId,
        number: file.number,
      },
      {
        onSuccess: (filename) => {
          updateBookImport({
            ...file,
            destinationFileName: filename,
            loading: false,
          });
        },
        onError: () => {
          toast.error("An error occurred while generating filename");
          updateBookImport({
            ...file,
            loading: false,
          });
        },
      }
    );
  }, [file.seriesId, file.number, file.matchId]);

  useMemo(() => {
    if (!file?.destinationFileName || !file?.seriesId) return;

    fileExists.mutate(
      {
        seriesId: file.seriesId,
        fileName: file.destinationFileName,
      },
      {
        onSuccess: (exists) => {
          updateBookImport({
            ...file,
            exists: exists,
          });
        },
        onError: () => {
          toast.error("An error occurred while checking if file exists");
        },
      }
    );
  }, [file.destinationFileName]);

  return (
    <tr key={row.id}>
      {row.getVisibleCells().map((cell) => (
        <td key={cell.id}>
          {flexRender(cell.column.columnDef.cell, cell.getContext())}
        </td>
      ))}
    </tr>
  );
};
